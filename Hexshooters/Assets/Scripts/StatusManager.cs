using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The different possible status effects that can affect a character.
/// </summary>
public enum StatusType
{
    Burn,
    Poison,
    Freeze,
    Break,
    Slow,
	Shield,
	Bound,
	Disabled,
    Stacking
}

/// <summary>
/// A status effect.  Keeps track of its time remaining and type.
/// </summary>
public class StatusEffect
{
    public StatusType m_type;
    public float m_timer;

    public StatusEffect(float initialTimer)
    {
        m_timer = initialTimer;
    }

}

public class StatusManager : MonoBehaviour
{

	public List<StatusEffect> m_effects = new List<StatusEffect>();

    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
	{
		//Debug.Log (m_effects.Count);
        for (int i = 0; i < m_effects.Count; ++i)
        {
			m_effects[i].m_timer -= Time.deltaTime;
			Debug.Log (m_effects[i].m_timer);
        }
        m_effects.RemoveAll(OutOfTime);
    }

    /// <summary>
    /// Adds an effect to the status list based on how that effect is added to the potential statuses.
    /// </summary>
    /// <param name="effect"></param>
    public void AddEffect(StatusEffect effect)
    {
        switch (effect.m_type)
        {
		case StatusType.Burn:
			if (m_effects.Contains (effect))
				effect.m_timer += 5;
			else
				m_effects.Add (effect);

			Debug.Log (m_effects.Count);
			break;
		case StatusType.Freeze:
		case StatusType.Poison:
		case StatusType.Bound:
			if (m_effects.Contains (effect))
				effect.m_timer += 8;
			else
				m_effects.Add (effect);

			Debug.Log (m_effects.Count);
			break;
		case StatusType.Disabled:
			if (m_effects.Contains(effect))
				effect.m_timer += 8;
			else
				m_effects.Add(effect);
			break;
            case StatusType.Break:
                if (m_effects.Contains(effect))
                    effect.m_timer += 10;
                else
                    m_effects.Add(effect);
                break;
            case StatusType.Slow:
                if (m_effects.Contains(effect))
                    effect.m_timer += 4;
                else
                    m_effects.Add(effect);
                break;
            case StatusType.Shield:
                if (m_effects.Contains(effect))
                    effect.m_timer += 5;
                else
                    m_effects.Add(effect);
                break;
            case StatusType.Stacking:
                if (m_effects.Contains(effect))
                    effect.m_timer += 6;
                else
                    m_effects.Add(effect);
                break;
            default:
                m_effects.Add(effect);
                break;
        }
    }

    /// <summary>
    /// Confirms whether or not the current status effect is affecting the character.
    /// </summary>
    /// <param name="type"></param>
    public bool IsAffected(StatusType type)
    {
        for (int i = 0; i < m_effects.Count; ++i)
        {
            if (m_effects[i].m_type == type)
            {
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// Function that checks if a status effect is out of time.
    /// </summary>
    /// <param name="effect">Effect being checked.</param>
    /// <returns></returns>
    private static bool OutOfTime(StatusEffect effect)
    {
		Debug.Log ("Fuck this noise");
        return effect.m_timer <= 0.0f;
    }
}
