using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class statsFileLoader : MonoBehaviour
{
    public TextAsset weapon_base_data;
    public TextAsset ship_base_data;
    public GunDataInterface[] getGunList()
    {
        GunDataListInterface json = JsonUtility.FromJson<GunDataListInterface>(weapon_base_data.text);
        return json.guns;
    }
    public GunDataInterface getGunByName(string name)
    {
        GunDataInterface[] gunList = getGunList();
        GunDataInterface funcout = null;

        foreach (GunDataInterface gun in gunList)
        {
            if(gun.gunName == name)
            {
                funcout = gun;
                break;
            }
        }
        return funcout;
    }
    public ShipDataInterface[] getShipList()
    {
        ShipDataListInterface json = JsonUtility.FromJson<ShipDataListInterface>(ship_base_data.text);
        return json.ships;
    }
    public ShipDataInterface getShipByType(string type)
    {
        ShipDataInterface[] shipList = getShipList();
        ShipDataInterface funcout = null;

        foreach (ShipDataInterface ship in shipList)
        {
            if (ship.shipType == type)
            {
                funcout = ship;
                break;
            }
        }
        return funcout;
    }
}
