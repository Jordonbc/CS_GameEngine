using System;
namespace GameEngine
{
    partial class EngineClass
    {
        /// <summary>Creates a User interface object to the game such as text.</summary>
        ///<param name = "UIObj" > User defined UI object.</param>
        /// <example>
        /// <code>
        /// GameObject HealthObject = new GameObject("HealthObject");
        /// 
        /// TextComponent tc = new TextComponent(locallyDefinedEngineObject, HealthObject, 0, 20, Color.White, "Health: 100%", new FontFamily("Arial"), 12);
        /// 
        /// locallyDefinedEngineObject.CreateUIObject(HealthObject);
        /// </code>
        /// </example>
        public void CreateUIObject(GameObject UIObj)
        {
            UIObjects.Add(UIObj);
        }

        /// <summary>Creates an in-game object for example, the player, something that can interact or just a visual element.</summary>
        ///<param name = "GameObj" > User defined game object.</param>
        /// <example>
        /// <code>
        /// GameObject Player = new GameObject("Player");
        /// </code>
        /// </example>
        public void CreateObject(GameObject GameObj)
        {
            GameObjects.Add(GameObj);
        }

        /// <summary>Destroys a game object by name. Removes them from the render list and destorys all links to the object except for locally defined</summary>
        ///<param name = "Name" > User defined game object.</param>
        /// <example>
        /// <code>
        /// Destroy player when it is hit
        /// if (player.hit)
        /// {
        ///     DestroyObjectByName("Player");
        /// }
        /// </code>
        /// </example>
        public void DestroyObjectByName(string Name)
        {
            GameObjects.RemoveAt(GetObjectIDByName(Name));
        }

        /// <summary>Destroys a game object by ID. Removes them from the render list and destorys all links to the object except for locally defined</summary>
        ///<param name = "ID" > User defined game object ID.</param>
        /// <example>
        /// <code>
        /// Destroy player when it is hit
        /// if (player.hit)
        /// {
        ///     DestroyObjectByID(Player.id);
        /// }
        /// </code>
        /// </example>
        public void DestroyObjectByID(int ID)
        {
            GameObjects.RemoveAt(ID);
        }

        /// <summary>Gets an object by ID</summary>
        ///<param name = "index" > User defined game object ID.</param>
        /// <example>
        /// <code>
        /// GameObject player = GetObjectByID(PLAYER_ID)
        /// </code>
        /// </example>
        /// <Returns>Returns a game object from specified ID.</Returns>
        public GameObject GetObjectbyID(int index)
        {
            GameObject GameObj = GameObjects[index];
            return GameObj;
        }

        /// <summary>Gets an object by name</summary>
        ///<param name = "Name" > User defined game object name.</param>
        /// <example>
        /// <code>
        /// GameObject player = GetObjectByName("player")
        /// </code>
        /// </example>
        /// <Returns>Returns a game object from specified name.</Returns>
        public GameObject GetObjectByName(String Name)
        {
            return GameObjects[GetObjectIDByName(Name)];
        }

        public GameObject GetUIObjectByName(String Name)
        {
            return UIObjects[GetObjectIDByName(Name)];
        }

        public GameObject GetObjectByID(int ID)
        {
            if (ID < GameObjects.Count)
            {
                return GameObjects[ID];
            }
            else
            {
                PrintText(debugType.Error, "Cannot find specified object '" + ID.ToString() + "'");
                return GameObjects[0];
            }
        }

        public int GetObjectIDByName(string Name)
        {
            bool found = false;
            int GameObjID = 0;
            for (int i = 0; i < GameObjects.Count; i++)
            {
                //Console.WriteLine("OBJECT: '" + GameObjects[i].Name + "', ID: " + i.ToString());
                if (GameObjects[i].Name == Name)
                {
                    if (debug == debugType.Debug) { PrintText(debugType.Debug, "Object Found!"); }
                    found = true;
                    GameObjID = i;
                    break;
                }
            }


            for (int i = 0; i < UIObjects.Count; i++)
            {
                //Console.WriteLine("OBJECT: '" + UIObjects[i].Name + "', ID: " + i.ToString());
                if (UIObjects[i].Name == Name)
                {
                    if (debug == debugType.Debug) { PrintText(debugType.Debug, "Object Found!"); }
                    //PrintText(debugType.Debug, "Object Found!: " + UIObjects[i].Name);
                    found = true;
                    GameObjID = i;
                    break;
                }
            }

            if (found)
            {
                return GameObjID;
            }
            else
            {
                throw new InvalidObjectException("Cannot Find Specified Game Object '" + Name + "'");
            }
        }

        public void SetObject(int index, GameObject gameObj) => GameObjects[index] = gameObj;

        public void SetObjectByName(string Name, GameObject gameObj)
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == Name)
                {
                    GameObjects[i] = gameObj;
                    break;
                }
            }
        }
    }
}
