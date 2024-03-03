using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class RebindAction : MonoBehaviour
{
    [Header("ActionReference")]
    public InputActionReference ActionReference;
    public string BindingId;
    public InputBinding.DisplayStringOptions DisplayStringOptions;

    [Header("UI")]
    public TextMeshProUGUI ActionLabel;
    public TextMeshProUGUI BindingText;
    public GameObject RebindOverlay;
    public TextMeshProUGUI RebindText;

    [Header("Events")]
    public UpdateBindingUIEvent UpdateBindingEvent = new UpdateBindingUIEvent();
    public InteractiveRebindEvent RebindStartEvent = new InteractiveRebindEvent();
    public InteractiveRebindEvent RebindStopEvent = new InteractiveRebindEvent();

    // private
    private InputActionRebindingExtensions.RebindingOperation RebindOperation;
    private static List<RebindAction> RebindActions;

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateActionLabel();
        //UpdateBindingDisplay();
    }

    public InputActionRebindingExtensions.RebindingOperation ongoingRebind => RebindOperation;

    public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
    {
        bindingIndex = -1;

        action = ActionReference?.action;
        if (action == null)
            return false;

        if (string.IsNullOrEmpty(BindingId))
            return false;

        // Look up binding index.
        var bindingId = new Guid(BindingId);
        bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
        if (bindingIndex == -1)
        {
            Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
            return false;
        }

        return true;
    }

    public void UpdateBindingDisplay()
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        // Get display string from action.
        var action = ActionReference?.action;
        if (action != null)
        {
            var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == BindingId);
            if (bindingIndex != -1)
                displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, DisplayStringOptions);
        }

        // Set on label (if any).
        if (BindingText != null)
            BindingText.text = displayString;

        // Give listeners a chance to configure UI in response.
        UpdateBindingEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
    }

    public void ResetToDefault()
    {
        if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            return;

        if (action.bindings[bindingIndex].isComposite)
        {
            // It's a composite. Remove overrides from part bindings.
            for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                action.RemoveBindingOverride(i);
        }
        else
        {
            action.RemoveBindingOverride(bindingIndex);
        }
        UpdateBindingDisplay();
    }

    public void StartInteractiveRebind()
    {
        if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            return;

        // If the binding is a composite, we need to rebind each part in turn.
        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
        }
        else
        {
            PerformInteractiveRebind(action, bindingIndex);
        }
    }

    private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        RebindOperation?.Cancel(); // Will null out RebindOperation.

        void CleanUp()
        {
            RebindOperation?.Dispose();
            RebindOperation = null;
        }

        // Configure the rebind.
        RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnCancel(
                operation =>
                {
                    RebindStopEvent?.Invoke(this, operation);
                    RebindOverlay?.SetActive(false);
                    UpdateBindingDisplay();
                    CleanUp();
                })
            .OnComplete(
                operation =>
                {
                    RebindOverlay?.SetActive(false);
                    RebindStopEvent?.Invoke(this, operation);
                    UpdateBindingDisplay();
                    CleanUp();

                    // If there's more composite parts we should bind, initiate a rebind
                    // for the next part.
                    if (allCompositeParts)
                    {
                        var nextBindingIndex = bindingIndex + 1;
                        if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                            PerformInteractiveRebind(action, nextBindingIndex, true);
                    }
                });

        // If it's a part binding, show the name of the part in the UI.
        var partName = default(string);
        if (action.bindings[bindingIndex].isPartOfComposite)
            partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

        // Bring up rebind overlay, if we have one.
        RebindOverlay?.SetActive(true);
        if (RebindText != null)
        {
            var text = !string.IsNullOrEmpty(RebindOperation.expectedControlType)
                ? $"{partName}Waiting for {RebindOperation.expectedControlType} input..."
                : $"{partName}Waiting for input...";
            RebindText.text = text;
        }

        // If we have no rebind overlay and no callback but we have a binding label,
        // temporarily set the binding label to "<Waiting>".
        if (RebindOverlay == null && RebindText == null && RebindStartEvent == null && BindingText != null)
            BindingText.text = "<Waiting...>";

        // Give listeners a chance to act on the rebind starting.
        RebindStartEvent?.Invoke(this, RebindOperation);

        RebindOperation.Start();
    }

    protected void OnEnable()
    {
        if (RebindActions == null)
            RebindActions = new List<RebindAction>();
        RebindActions.Add(this);
        if (RebindActions.Count == 1)
            InputSystem.onActionChange += OnActionChange;
    }

    protected void OnDisable()
    {
        RebindOperation?.Dispose();
        RebindOperation = null;

        RebindActions.Remove(this);
        if (RebindActions.Count == 0)
        {
            RebindActions = null;
            InputSystem.onActionChange -= OnActionChange;
        }
    }

    // When the action system re-resolves bindings, we want to update our UI in response. While this will
    // also trigger from changes we made ourselves, it ensures that we react to changes made elsewhere. If
    // the user changes keyboard layout, for example, we will get a BoundControlsChanged notification and
    // will update our UI to reflect the current keyboard layout.
    private static void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.BoundControlsChanged)
            return;

        var action = obj as InputAction;
        var actionMap = action?.actionMap ?? obj as InputActionMap;
        var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

        for (var i = 0; i < RebindActions.Count; ++i)
        {
            var component = RebindActions[i];
            var referencedAction = component.ActionReference?.action;
            if (referencedAction == null)
                continue;

            if (referencedAction == action ||
                referencedAction.actionMap == actionMap ||
                referencedAction.actionMap?.asset == actionAsset)
                component.UpdateBindingDisplay();
        }
    }

    // We want the label for the action name to update in edit mode, too, so
    // we kick that off from here.
#if UNITY_EDITOR
    protected void OnValidate()
    {
        UpdateActionLabel();
        UpdateBindingDisplay();
    }
#endif

    private void UpdateActionLabel()
    {
        if (ActionLabel != null)
        {
            var action = ActionReference?.action;
            ActionLabel.text = action != null ? action.name : string.Empty;
        }
    }

    [Serializable]
    public class UpdateBindingUIEvent : UnityEvent<RebindAction, string, string, string>
    {
    }

    [Serializable]
    public class InteractiveRebindEvent : UnityEvent<RebindAction, InputActionRebindingExtensions.RebindingOperation>
    {
    }
}