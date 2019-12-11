using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class TestUtils
    {
        public static IEnumerator LoadScene()
        {
            SceneManager.LoadScene("Virtual Juggling");
            yield return new WaitForSeconds(0.1f);

            //    AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Virtual Juggling", LoadSceneMode.Single);
            //    while (!asyncLoadLevel.isDone)
            //    {
            //        // yield return null;
            //        yield return new WaitForSeconds(0.1f);
            //    }
        }

        public static void Juggle(string siteswap, int rounds)
        {
            uint left = 1;
            uint right = 2;
            int green = 1;
            int blue = 2;
            int red = 3;

            switch (siteswap)
            {
                case "3":
                    GameEvents.current.Catch(left, green);
                    GameEvents.current.Catch(right, blue);

                    for (int i = 0; i < rounds; i++)
                    {
                        GameEvents.current.Catch(left, red);
                        GameEvents.current.Catch(right, green);
                        GameEvents.current.Catch(left, blue);
                        GameEvents.current.Catch(right, red);
                        GameEvents.current.Catch(left, green);
                        GameEvents.current.Catch(right, blue);
                    }
                    break;
                case "531":
                    GameEvents.current.Catch(left, green);
                    GameEvents.current.Catch(right, blue);
                    GameEvents.current.Catch(left, red);
                    GameEvents.current.Catch(right, red); // 1
                    GameEvents.current.Catch(left, blue); // 3
                    GameEvents.current.Catch(right, green); // 5
                    break;
                case "423":
                    // First two are caught
                    GameEvents.current.Catch(right, green);
                    GameEvents.current.Catch(left, blue);

                    GameEvents.current.Catch(right, red);
                    // Add two here
                    GameEvents.current.Catch(right, green); // 4 caught
                    GameEvents.current.Catch(left, red); // 3 caught
                    GameEvents.current.Catch(left, blue);

                    GameEvents.current.Catch(right, red);
                    // Add two here
                    GameEvents.current.Catch(right, green); // 4 caught
                    GameEvents.current.Catch(left, red); // 3 caught
                    GameEvents.current.Catch(left, blue);

                    GameEvents.current.Catch(right, red);
                    // Add two here
                    GameEvents.current.Catch(right, green); // 4 caught
                    GameEvents.current.Catch(left, red); // 3 caught
                    GameEvents.current.Catch(left, blue);


                    GameEvents.current.Catch(right, red);
                    // Add two here            
                    GameEvents.current.Catch(right, green); // 4 caught            
                    GameEvents.current.Catch(left, red); // 3 caught            
                    GameEvents.current.Catch(left, blue);
                    break;
                case "40":
                    GameEvents.current.Catch(right, green);
                    GameEvents.current.Catch(right, blue);
                    GameEvents.current.Catch(right, green);
                    GameEvents.current.Catch(right, blue);
                    GameEvents.current.Catch(right, green);
                    GameEvents.current.Catch(right, blue);
                    GameEvents.current.Catch(right, green);
                    GameEvents.current.Catch(right, blue);
                    GameEvents.current.Catch(right, green);
                    GameEvents.current.Catch(right, blue);
                    break;
            }
        }
    }
}
