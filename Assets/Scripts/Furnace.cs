﻿// <copyright file="Furnace.cs" company="Mewzor Holdings Inc.">
//     Copyright (c) Mewzor Holdings Inc. All rights reserved.
// </copyright>
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[DisallowMultipleComponent]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.  We want to have public methods.")]
public class Furnace : MonoBehaviour
{
    public List<FurnaceItem> Contents = new List<FurnaceItem>();

    public int Fuel = 100;

    public float LastFueledTime;

    public void Add(Item item)
    {
        Contents.Add(new FurnaceItem(item));
    }

    private Provider provider;
    
    private void Awake()
    {
        provider = GetComponent<Provider>();
    }

    /// <summary>
    /// moves finished items to Provider
    /// </summary>
    private void FixedUpdate()
    {
        if (Fuel <= 0)
        {
            if (LastFueledTime < Time.timeSinceLevelLoad - 30)
            {
                GetComponent<Provider>().DestroyOnEmpty = true;
            }

            return;
        }

        // A fueled fire doesn't peter out
        LastFueledTime = Time.timeSinceLevelLoad;

        for (int i = 0; i < Contents.Count; i++)
        {
            Contents[i].Cook();
            Fuel--;

            // It takes at least two rounds to cook
            if (Contents[i].Cooked > 240)
            {
                string name = Contents[i].Item.Name.Split('(')[0].TrimEnd(' ');
                
                provider.Add(name + " (Cooked)", 1, 1);
                Contents.RemoveAt(i);
                i--;
            }
        }

        if (Contents.Count == 0)
        {
            Fuel--;
        }
    }
}
