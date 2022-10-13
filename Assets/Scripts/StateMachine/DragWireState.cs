using DG.Tweening;
using StateMachine;
using UnityEngine;

public class DragWireState : InputStateBase
{
    private readonly WireEndCube _selectedCube;
   
    private static RaycastHit[] _hits;
    public DragWireState(WireEndCube cube)
    {
        _selectedCube = cube;
        _hits = new RaycastHit[15];

    }
    //private V3 initPos or initLocalPos;

    public override void OnEnter()
    {
        base.OnEnter();
        //store initPos
        if(!_selectedCube.isAllowedToDrag)
        {
            ExitState();
            return;
        }
        var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
        if (!Physics.Raycast(ray, out var hit, RaycastDistance))
        {
            ExitState();
            return;
        }
        if(!hit.collider.CompareTag("Draggable"))
        {
            ExitState();
            return;
        }

        
        _selectedCube.StartDragging(hit.transform, ray.origin);
    }

    public override void Execute()
    {
        base.Execute();
        
        //if raycast doesnt hit anything,
        //or
        //no input/ input key up
        //then
        // go back to idle 
        
        //hit.transform.localPosition, move this in local XY
        
        
        //on trigger enter of cube
        // if cube intersect w correct cube,  assign idle state
        if(IsExitingCurrentState) return;
			
        if(!_selectedCube.isAllowedToDrag)
        {
            ExitState();
            return;
        }

        if(InputExtensions.GetFingerUp())
        {
            ExitState();
            return;
        }
        if(InputExtensions.GetFingerUp())
        {
            ExitState();
            return;
        }

        var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
        Physics.RaycastNonAlloc(ray, _hits, RaycastDistance);

        var worldPos = Vector3.zero;
        var didFindDraggable = false;
        foreach (var hit in _hits)
        {
            if(!hit.collider) continue;
            if(!hit.collider.CompareTag("Draggable")) continue;
            if(_selectedCube.TrySetSwappable(hit.transform)) break;

            worldPos = hit.point;
        }
        _selectedCube.Drag(ray.direction);
    }
    

    public override void OnExit()
    {
        base.OnExit();
        if (_selectedCube.didsnap == false)
        {
            Debug.Log("Calling");
            _selectedCube.transform.DOLocalMove(_selectedCube.startLocalPoint, .05f).SetEase(Ease.Linear);
        }
        //if i have a selected cube
        //dotween positions of both the selected cube and (if any) collided cube to appropriate position
        
        //this code will be in the WireEndCube class
        /* if (has collided cube)
         selectedCube & collided cube .dotween to dist/2
         else
         selectedCube go back to initPos
        */
    }

   
}
