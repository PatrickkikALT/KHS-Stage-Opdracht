using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipmentController : MonoBehaviour {
  public EquipmentManager manager;

  public void LeftPrimaryFire(InputAction.CallbackContext ctx) {
    if (ctx.started) {
      HandleHandUse(EquipmentSlot.LeftHand, EquipmentSlot.RightHand, true);
    }
    else if (ctx.canceled) {
      HandleHandUse(EquipmentSlot.LeftHand, EquipmentSlot.RightHand, false);
    }
  }

  public void RightPrimaryFire(InputAction.CallbackContext ctx) {
    if (ctx.started) {
      HandleHandUse(EquipmentSlot.RightHand, EquipmentSlot.LeftHand, true);
    }
    else if (ctx.canceled) {
      HandleHandUse(EquipmentSlot.LeftHand, EquipmentSlot.RightHand, false);
    }
  }

  public void LeftSecondaryFire(InputAction.CallbackContext ctx) {
    if (ctx.started) {
      HandleSecondary(EquipmentSlot.LeftHand);
    }
  }

  public void RightSecondaryFire(InputAction.CallbackContext ctx) {
    if (ctx.started) {
      HandleSecondary(EquipmentSlot.RightHand);
    }
  }

  public void DropPrimary(InputAction.CallbackContext ctx) => manager.Unequip(EquipmentSlot.LeftHand);

  public void DropSecondary(InputAction.CallbackContext ctx) => manager.Unequip(EquipmentSlot.RightHand);
  

  private void HandleHandUse(EquipmentSlot slot, EquipmentSlot opposite, bool held) {
    var instance = manager.GetItemInstance(slot);
    if (!instance) return;
    
    if (instance.TryGetComponent(out AmmoClip _)) {
      TryPerformReload(slot, opposite);
      return;
    }
    if (held) {
      if (instance.TryGetComponent(out IUsableItem usable)) {
        usable.UsePrimary();
      }
    }
    else {
      if (instance.TryGetComponent(out IUsableItem usable)) {
        usable.UsePrimaryStopped();
      }
    }
  }

  private void HandleSecondary(EquipmentSlot slot) {
    var instance = manager.GetItemInstance(slot);
    if (!instance) return;
    
    if (instance.TryGetComponent(out IUsableItem usable)) {
      usable.UseSecondary();
    }
  }

  private void TryPerformReload(EquipmentSlot clipSlot, EquipmentSlot gunSlot) {
    var clipInstance = manager.GetItemInstance(clipSlot);
    var gunInstance = manager.GetItemInstance(gunSlot);
    
    if (!gunInstance || !clipInstance) return;
    
    if (gunInstance.TryGetComponent(out Gun gun) && clipInstance.TryGetComponent(out AmmoClip clip)) {
      gun.Reload(clip.bullets);
      manager.Unequip(clipSlot);
    }
  }
}