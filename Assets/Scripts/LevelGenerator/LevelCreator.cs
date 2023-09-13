using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MyGame
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField]
        Transform _parent;

        [SerializeField] GameObject _block1;
        [SerializeField] GameObject _block2;
        [SerializeField] GameObject _block3;
        [SerializeField] GameObject _block4;
        [SerializeField] GameObject _block5;
        [SerializeField] GameObject _block6;
        [SerializeField] GameObject _block7;
        [SerializeField] GameObject _block8;
        [SerializeField] GameObject _block9;
        [SerializeField] GameObject _blockA;
        [SerializeField] GameObject _blockB;
        [SerializeField] GameObject _blockC;
        [SerializeField] GameObject _blockD;

        [SerializeField]
        float _finishYOffset = -0.01658511f;
        [SerializeField] GameObject _blockFinish;

        [SerializeField]
        GameObject _blockRock;

        [SerializeField]
        float _spawnPointYPos = 0.806f;

        TextAsset _levelTxt;

        void Awake()
        {
            _levelTxt = Resources.Load<TextAsset>("level" + GameProgressStatic.LevelNumber);
#if TEST
            _levelTxt = Resources.Load<TextAsset>("test");
#endif
            string fs = _levelTxt.text;
            fs.Replace("\r", "");
            string[] fLines = Regex.Split(fs, "\n");

            float zSize = 2f;
            float xSize = 2f;
            float ySize = 1f;

            int columns = fLines[0].Length;

            Vector3 spawnPositionLeftBottom = Vector3.left * xSize * ((float)columns - 1f) / 2f 
                + Vector3.down * 0.5f * ySize
                + Vector3.back * zSize;
            for (int i = 0; i < fLines.Length; i++)
            {
                Vector3 spawnPosition = spawnPositionLeftBottom + Vector3.forward * zSize * i;
                foreach (char c in fLines[fLines.Length - 1 - i])
                {
                    GameObject spawnPoint;
                    switch (c)
                    {
                        case '1':
                            Instantiate(_block1, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '2':
                            Instantiate(_block2, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '3':
                            Instantiate(_block3, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '4':
                            Instantiate(_block4, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '5':
                            Instantiate(_block5, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '6':
                            Instantiate(_block6, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '7':
                            Instantiate(_block7, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '8':
                            Instantiate(_block8, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case '9':
                            Instantiate(_block9, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case 'a':
                            Instantiate(_blockA, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case 'b':
                            Instantiate(_blockB, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case 'c':
                            Instantiate(_blockC, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case 'd':
                            Instantiate(_blockD, spawnPosition, Quaternion.identity, _parent);
                            break;
                        case 'F':
                            Instantiate(_block5, spawnPosition, Quaternion.identity, _parent);
                            Instantiate(_blockFinish, spawnPosition + Vector3.up * (_finishYOffset + 0.5f * ySize), Quaternion.identity, _parent);
                            break;
                        case 'R':
                            Instantiate(_block5, spawnPosition, Quaternion.identity, _parent);
                            Instantiate(_blockRock, spawnPosition + Vector3.up, Quaternion.Euler(-90, 45, 0), _parent);
                            break;
                        case 'e':
                            Instantiate(_block5, spawnPosition, Quaternion.identity, _parent);
                            spawnPoint = new GameObject("SpawnPoint");
                            spawnPoint.transform.position = new Vector3(spawnPosition.x, _spawnPointYPos, spawnPosition.z);
                            spawnPoint.tag = "SpawnPoint";
                            break;
                        case 'E':
                            Instantiate(_block5, spawnPosition, Quaternion.identity, _parent);
                            spawnPoint = new GameObject("SpawnPointBig");
                            spawnPoint.transform.position = new Vector3(spawnPosition.x, _spawnPointYPos * 1.1f, spawnPosition.z);
                            spawnPoint.tag = "SpawnPointBig";
                            break;
                    }
                    spawnPosition += Vector3.right * xSize;
                }
            }
        }
    }
}