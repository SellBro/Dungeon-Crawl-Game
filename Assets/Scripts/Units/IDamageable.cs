using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }
}
