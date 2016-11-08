using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ResultScene;

namespace ResultScene
{
    public class CharacterStatusManager: MonoBehaviour
    {
        [SerializeField]
        private Text exp;
        [SerializeField]
        private CharacterStatusPanel status;

        public void Initialize(int exp)
        {
            this.exp.text = exp.ToString();

            // NOTE:foreachで回す
            {
                var obj = Instantiate(status);
                obj.transform.SetParent(transform);
                obj.Initialize(null, "", "", 0);
            }
        }
    }
}