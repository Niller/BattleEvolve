using Sources.Utilities;
using UnityEngine;

namespace Sources.Controllers
{
    public class GameInputController : MonoBehaviour
    {
        void Update()
        {
            /*if (Contexts.sharedInstance.game.combat.IsBlockedInput)
            return;*/

            TestUpdateButtons();

            UpdateMouseState();
        }

        void TestUpdateButtons()
        {
            /*if (Input.GetKeyUp(KeyCode.A))
        {
            Contexts.sharedInstance.game.ReplaceCombat(true, false, 10 , new Vector2(52,32));
        }*/
        }

        void UpdateMouseState()
        {
            var input = Contexts.sharedInstance.input;

            if (CheckGUIInterception(Input.mousePosition))
            {
                input.ReplaceMouseInputState(false, false, null, true);
                return;
            }
            else
            {
                var groundPosition = GetGroundMousePosition(Input.mousePosition);
                input.ReplaceMouseInputState(Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1), groundPosition, false);
            }

            /*var interactableObject = GetInteractableObject(Input.mousePosition);
        var groundPosition = GetGroundMousePosition(Input.mousePosition);

        if (groundPosition != null)
        {
            input.ReplaceMouseInputState(Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1), groundPosition, false, interactableObject);
        }
        else
        {
            input.ReplaceMouseInputState(false, false, null, false, interactableObject);
        }*/
        }

        private bool CheckGUIInterception(Vector2 mousePos)
        {
            Collider2D[] col = Physics2D.OverlapPointAll(mousePos);
            return col.Length > 0;
        }

        /*private InteractableComponent GetInteractableObject(Vector2 screenMousePosition)
    {
        var ray = Camera.main.ScreenPointToRay(screenMousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer(GameConsts.INTERACTABLE_OBJECTS)))
        {
            return hitInfo.transform.GetComponent<InteractableEntityDescription>().entity.interactable;
        }
        return null;
    }*/

        private Vector2? GetGroundMousePosition(Vector2 screenMousePosition)
        {
            var ray = Camera.main.ScreenPointToRay(screenMousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer(GameConsts.GroundLayer)))
            {
                return new Vector2(hitInfo.point.x, hitInfo.point.z);
            }
            return null;
        }
    }
}
