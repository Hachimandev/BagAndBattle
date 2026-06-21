using UnityEngine;
using UnityEngine.InputSystem; 

public class CombatTester : MonoBehaviour
{
    public PlayerEntity player;
    public MonsterEntity monster;

    void Update()
    {
   

        // 1. Nhấn nút S: Player đánh Monster
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (monster != null) monster.TakeDamage(15);
        }

        // 2. Nhấn nút D: Monster đánh Player
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (player != null) player.TakeDamage(10);
        }

        // 3. Nhấn nút H: Hồi máu cho Player
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            if (player != null) player.Heal(20);
        }

        // --- TEST AUTO BATTLE ---
        // Nhấn Enter để bắt đầu Auto Battle
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Debug.Log("[CombatTester] Phím Enter được nhấn! Đang gọi StartCombat...");
            if (CombatManager.Instance != null)
            {
                CombatManager.Instance.StartCombat();
            }
            else
            {
                Debug.LogError("[CombatTester] Không tìm thấy CombatManager.Instance! Hãy chắc chắn bạn đã tạo GameObject có gắn CombatManager.");
            }
        }

        // Nhấn 1, 2, 3 để đổi tốc độ
        if (Keyboard.current.digit1Key.wasPressedThisFrame) 
        {
            Debug.Log("[CombatTester] Đổi tốc độ x1");
            if (CombatManager.Instance != null) CombatManager.Instance.SetGameSpeed(1f);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame) 
        {
            Debug.Log("[CombatTester] Đổi tốc độ x2");
            if (CombatManager.Instance != null) CombatManager.Instance.SetGameSpeed(2f);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame) 
        {
            Debug.Log("[CombatTester] Đổi tốc độ x3");
            if (CombatManager.Instance != null) CombatManager.Instance.SetGameSpeed(3f);
        }
    }
}