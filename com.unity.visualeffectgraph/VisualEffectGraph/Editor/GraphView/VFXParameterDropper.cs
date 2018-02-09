using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.VFX.UI
{
    interface IParameterDropTarget
    {
        void OnDragUpdated(IMGUIEvent evt, VFXParameterController parameter);
        void OnDragPerform(IMGUIEvent evt, VFXParameterController parameter);
    }

    class ParameterDropper : Manipulator
    {
        public ParameterDropper()
        {
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<IMGUIEvent>(OnIMGUIEvent);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<IMGUIEvent>(OnIMGUIEvent);
        }

        void OnIMGUIEvent(IMGUIEvent e)
        {
            Event evt = e.imguiEvent;
            if (evt.type != EventType.DragUpdated && evt.type != EventType.DragPerform)
                return;
            var pickElem = target.panel.Pick(target.BoundToGlobal(evt.mousePosition));
            IParameterDropTarget dropTarget = pickElem != null ? pickElem.GetFirstOfType<IParameterDropTarget>() : null;

            if (dropTarget == null)
                return;

            VFXParameterController dragData = DragAndDrop.GetGenericData(VisualEffectAssetEditor.VFXParameterDragging) as VFXParameterController;

            if (dragData == null)
                return;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                {
                    dropTarget.OnDragUpdated(e, dragData);
                }
                break;
                case EventType.DragPerform:
                {
                    dropTarget.OnDragPerform(e, dragData);
                }
                break;
            }
        }
    }
}
