using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace SpaceScrapper
{
    [CreateAssetMenu(fileName = "new Faction", menuName = "Custom Data/Faction")]
    public class Faction : ScriptableObject
    {
        [SerializeField, Tooltip("Are they hostile by default if the entity doesnt belong to a faction?")]
        [FormerlySerializedAs("playerNoFactionStanding")]
        private Standing defaultStandingWithFactionlessEntities = Standing.UNDEFINED;

        /// <summary>
        /// Define how the players standings towards other factions should be by default
        /// </summary>
        [SerializeField, Tooltip("The default standing with other factions.")]
        private List<FactionStanding> factionStandings;

        //NOTE: not sure if handling this through a dictionary <Faction, Standing> could be any faster
        /// <summary>
        /// Checks this factions standing with another to determine hostility and attackability.
        /// </summary>
        /// <param name="other">The other faction.</param>
        public bool IsHostileTowards(Faction other)
        {
            //entity doesnt belong to faction -> should be a player!
            //Its important that ALL entities other than player are assigned to a faction.
            if(other == null)
            {
                return defaultStandingWithFactionlessEntities is Standing.Hostile;
            }
            FactionStanding fs = factionStandings.Find(x => x.other == other);
            if (fs != null)
            {
                return fs.standing is Standing.Hostile;
            }
            //UNDEFINED or NULL
            return false;
        }

        /// <summary>
        /// Whether this faction can deal damage to the other factions members.
        /// </summary>
        /// <param name="other">The other faction.</param>
        /// <returns>True for Hostile or UNDEFINED (neutral) standing.</returns>
        public bool CanDamage(Faction other)
        {
            //entity doesnt belong to faction -> should be a player!
            //Its important that ALL entities other than player are assigned to a faction.
            if (other is null)
            {
                return defaultStandingWithFactionlessEntities != Standing.Friendly;
            }
            FactionStanding fs = factionStandings.Find(x => x.other == other);
            if (fs != null)
            {
                return fs.standing != Standing.Friendly;
            }
            //UNDEFINED or NULL
            return true;
        }

        /// <summary>
        /// CanDamage and IsHostileTowards combined into one call.
        /// </summary>
        /// <param name="canAttack">defines whether this faction can attack the other.</param>
        /// <returns>Hostility bool</returns>
        public bool IsHostileAndCanAttack(Faction other, out bool canAttack)
        {
            //entity doesnt belong to faction -> should be a player!
            //Its important that ALL entities other than player are assigned to a faction.
            if (other is null)
            {
                if(defaultStandingWithFactionlessEntities is Standing.Friendly)
                {
                    canAttack = false;
                    return false;
                }
                if (defaultStandingWithFactionlessEntities is Standing.Hostile)
                {
                    canAttack = true;
                    return true;
                }
                canAttack = true;
                return false;
            }
            FactionStanding fs = factionStandings.Find(x => x.other == other);
            if (fs != null)
            {
                if(fs.standing is Standing.Friendly)
                {
                    canAttack = false;
                    return false;
                }
                if(fs.standing is Standing.Hostile)
                {
                    canAttack = true;
                    return true;
                }
            }
            //UNDEFINED or NULL
            canAttack = true;
            return false;
        }

        /// <summary>
        /// Adds a reference to another Faction if it doesnt exist yet.
        /// </summary>
        /// <param name="sender">The faction that called the method (Always use [this])</param>
        private void AddReferenceTo(Faction sender)
        {
            if (factionStandings.Find(x => x.other == sender) == null)
            {
                factionStandings.Add(new FactionStanding() { other = sender, standing = Standing.UNDEFINED });
            }
        }

        /// <summary>
        /// Changes a standing with another faction.
        /// </summary>
        /// <param name="sender">The faction that called the method. (this)</param>
        /// <param name="s">The updated standing.</param>
        private void SetStandingWithFaction(Faction sender, Standing s)
        {
            factionStandings.Find(x => x.other == sender).standing = s;
        }

        /// <summary>
        /// Sets the standing of BOTH of these factions to the new standing.
        /// Note: This is used at runtime to change standings between factions, and changes need to be saved. (mark dirty?)
        /// </summary>
        /// <param name="other">The other faction.</param>
        /// <param name="s">The updated standing.</param>
        public void OverrideStandingWithFaction(Faction other, Standing s)
        {
            this.SetStandingWithFaction(other, s);
            other.SetStandingWithFaction(this, s);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            //check whether the faction references itself, because that shouldnt happen.
            for (int i = 0; i < factionStandings.Count; i++)
            {
                if (factionStandings[i].other == this)
                {
                    factionStandings.RemoveAt(i);
                    Debug.LogWarning("Factions should not reference themselves!");
                    return;
                }
                else
                {
                    //make sure that they reference each other.
                    factionStandings[i].other.AddReferenceTo(this);
                    //if one factions standing is changed, it should also be changed on the other.
                    factionStandings[i].other.SetStandingWithFaction(this, factionStandings[i].standing);
                }
            }
        }
#endif
    }

    /// <summary>
    /// Defines how one faction is viewed by another.
    /// </summary>
    [Serializable]
    public class FactionStanding
    {
        public Faction other;
        public Standing standing;
    }

    [Serializable]
    public enum Standing
    {
        UNDEFINED = 0,
        Hostile = 1,
        Friendly = 2,
    }
}