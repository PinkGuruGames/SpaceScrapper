using UnityEngine;

namespace SpaceScrapper
{
    public class SimpleEnemyEntity : LivingEntity
    {
        //[SerializeField]
        //private Faction faction; //The faction that this entity belongs to

        public override void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsHostileTowards(LivingEntity other)
        {
            throw new System.NotImplementedException();
        }

        protected override void Die()
        {
            base.Die();
            throw new System.NotImplementedException();
        }
    }
}
