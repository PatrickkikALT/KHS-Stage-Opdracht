using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickupController : MonoBehaviour {
  public float pickupRange = 2f;
  public Camera playerCamera;
  public EquipmentManager equipment;
  
  public void Pickup(InputAction.CallbackContext context) => TryPickup();

  private void TryPickup() {
    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, pickupRange)) {
      if (hit.collider.TryGetComponent(out PickupItem pickup)) {
        TryEquipItem(pickup);
      }
    }
  }

  private void TryEquipItem(PickupItem pickup) {
    EquipmentItem data = pickup.itemData;
    
    if (data.itemType == ItemType.Hat && !equipment.GetItemInstance(EquipmentSlot.Head)) {
      equipment.Equip(data, EquipmentSlot.Head);
      pickup.OnPickedUp(out GameObject obj);
      equipment.headWorldItem = obj;
      return;
    }
    if (!equipment.GetItemInstance(EquipmentSlot.LeftHand)) {
      equipment.Equip(data, EquipmentSlot.LeftHand);
      pickup.OnPickedUp(out GameObject obj);
      equipment.leftHandWorldItem = obj;
      return;
    }
    
    if (!equipment.GetItemInstance(EquipmentSlot.RightHand)) {
      equipment.Equip(data, EquipmentSlot.RightHand);
      pickup.OnPickedUp(out GameObject obj);
      equipment.rightHandWorldItem = obj;
      return;
    }
    
    
  }
}