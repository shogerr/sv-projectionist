using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class select_node : MonoBehaviour
{
    //Attach this script to a Dropdown GameObject
    public Dropdown dropdown;

    List<string> nodes = new List<string>()
    { "Select Node" , "DESKTOP-MJ6FIET" , "EAST SHEARWALL OG MC DAQ LOWER" , "EAST SHEARWALL OG MC DAQ UPPER", "FLOOR ZONE 1 DURING COSNT" , "Weather Station - OSU"
        , "ZONE 1 THERMISTORS"};


 

    void Start()
    {
        PopulateList();
    }

    void PopulateList() // need to add all the nodes we need
    {
        dropdown.AddOptions(nodes);
    }

    public void index_changed(int index)
    {
        GlobalVariables.selected_nodeID = index;
        switch (GlobalVariables.selected_nodeID) // list different sensors based on selected node
        {
            case 1:
                GlobalVariables.current_node = 1;
                break;
            case 2:
                GlobalVariables.current_node = 2;
                break;
            case 3:
                GlobalVariables.current_node = 3;
                break;
            case 4:
                GlobalVariables.current_node = 4;
                break;
            case 5:
                GlobalVariables.current_node = 5;
                break;
            case 6:
                GlobalVariables.current_node = 6;
                break;
            default:
                break;
        }
    }


}