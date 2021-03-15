using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemGeneratorManager))]
[RequireComponent(typeof(PlayerXpManager))]
[RequireComponent(typeof(UnitGeneratorManager))]
[RequireComponent(typeof(ProgressManager))]
[RequireComponent(typeof(MapGenerator))]

public class Managers : MonoBehaviour
{
    public static ItemGeneratorManager ItemGenerator { get; private set; }
    public static PlayerXpManager PlayerXP { get; private set; }
    public static UnitGeneratorManager UnitGenerator { get; private set; }
    public static ProgressManager progress { get; private set; }
    public static MapGenerator MapGenerator { get; private set; }

    //List for all the managers
    List<IGameManager> startSequence;

    void Awake()
    {
        //Let this game object persist between the scenes
        DontDestroyOnLoad(gameObject);

        //Get the manager components
        ItemGenerator = GetComponent<ItemGeneratorManager>();
        PlayerXP = GetComponent<PlayerXpManager>();
        UnitGenerator = GetComponent<UnitGeneratorManager>();
        progress = GetComponent<ProgressManager>();
        MapGenerator = GetComponent<MapGenerator>();

        //Initialize a new list with managers you got
        startSequence = new List<IGameManager>
        {
            ItemGenerator,
            PlayerXP,
            UnitGenerator,
            progress,
            MapGenerator
        };


        //Start coroutine
        StartCoroutine(StartupManagers());
    }

    IEnumerator StartupManagers()
    {
        //Start a startup methods in all of the managers
        foreach (IGameManager manager in startSequence)
            manager.Startup();

        //Wait for one frame
        yield return null;

        int numModules = startSequence.Count;
        int numReady = 0;

        //Keep looping until all managers are ready
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            //Check if each manager is started
            foreach (IGameManager manager in startSequence)
                if (manager.Status == ManagerStatus.Started)
                    numReady++;

            //If there is any progress
            if (numReady > lastReady)
            {
                //Display pregress
                Debug.Log($"Loading progress: {numReady}/{numModules}");
            }

            //Wait for one frame before next loop
            yield return null;
        }

        //Once everything is loaded display message
        Debug.Log("All managers started up");
    }
}
