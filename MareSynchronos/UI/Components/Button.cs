﻿using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using ImGuiNET;
using MareSynchronos.UI.VM;
using System.Numerics;

namespace MareSynchronos.UI.Components;

public class Button
{
    private const float _toolTipWidth = 300f;
    private readonly ButtonCommand _command;

    private Button(ButtonCommand command)
    {
        _command = command;
    }

    public static Button FromCommand(ButtonCommand command)
    {
        return new Button(command);
    }

    public void Draw()
    {
        var enabled = _command.StatefulCommandContent.Enabled.Invoke();
        var text = _command.StatefulCommandContent.ButtonText.Invoke();
        var icon = _command.StatefulCommandContent.Icon.Invoke();
        var tooltip = _command.StatefulCommandContent.Tooltip.Invoke();
        var color = _command.StatefulCommandContent.Foreground.Invoke();
        ImGui.PushStyleColor(ImGuiCol.Text, color);
        if (!enabled) ImGui.BeginDisabled();
        if (!string.IsNullOrEmpty(text) && icon != FontAwesomeIcon.None)
        {
            if (UiSharedService.IconTextButton(icon, text) && (!_command.RequireCtrl || (_command.RequireCtrl && UiSharedService.CtrlPressed())))
                _command.StatefulCommandContent.OnClick();
        }
        else if (icon != FontAwesomeIcon.None)
        {
            if (ImGuiComponents.IconButton(icon) && (!_command.RequireCtrl || (_command.RequireCtrl && UiSharedService.CtrlPressed())))
                _command.StatefulCommandContent.OnClick();
        }
        else if (!string.IsNullOrEmpty(text))
        {
            if (ImGui.Button(text) && (!_command.RequireCtrl || (_command.RequireCtrl && UiSharedService.CtrlPressed())))
                _command.StatefulCommandContent.OnClick();
        }
        else
        {
            throw new InvalidOperationException("Misdefined ButtonCommandContent");
        }
        if (!enabled) ImGui.EndDisabled();
        ImGui.PopStyleColor();
        if (!string.IsNullOrEmpty(tooltip) && ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
        {
            ImGui.BeginTooltip();
            UiSharedService.TextWrapped(tooltip, _toolTipWidth);
            if (_command.RequireCtrl)
            {
                ImGui.Separator();
                UiSharedService.ColorIcon(FontAwesomeIcon.ExclamationTriangle, ImGuiColors.DalamudYellow);
                ImGui.SameLine();
                UiSharedService.TextWrapped("Hold CTRL while pressing this button", _toolTipWidth);
            }
            ImGui.EndTooltip();
        }
    }

    public Vector2 GetSize()
    {
        var text = _command.StatefulCommandContent.ButtonText.Invoke();
        var icon = _command.StatefulCommandContent.Icon.Invoke();
        if (!string.IsNullOrEmpty(text) && icon != FontAwesomeIcon.None)
        {
            return UiSharedService.GetIconTextButtonSize(icon, text);
        }
        else if (icon != FontAwesomeIcon.None)
        {
            return UiSharedService.GetIconButtonSize(icon);
        }
        else if (!string.IsNullOrEmpty(text))
        {
            return ImGuiHelpers.GetButtonSize(text);
        }

        return Vector2.Zero;
    }
}