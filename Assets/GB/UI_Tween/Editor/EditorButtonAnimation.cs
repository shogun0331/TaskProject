using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace GB
{

    [CustomEditor(typeof(ButtonAnimation))]
    public class EditorButtonAnimation : Editor
    {

        string[] _menu = new string[4] { "None", "Scale", "Move", "Color" };


        public override void OnInspectorGUI()
        {
            var myButton = (ButtonAnimation)target;

            GB.EditorGUIUtil.DrawSectionStyleLabel("UI Button Tween");



            DrawScale(myButton);
          
            base.OnInspectorGUI();


            GB.EditorGUIUtil.Start_Horizontal();

            if (GB.EditorGUIUtil.DrawSyleButton("Copy Component"))
            {
                GUIUtility.systemCopyBuffer = myButton.gameObject.GetInstanceID().ToString();

                myButton.gameObject.GetInstanceID().ToString().GBLog("InstanceID", Color.green);

            }

            if (GB.EditorGUIUtil.DrawSyleButton("Load Component"))
            {
                try
                {
                    var obj = FindObject(int.Parse(GUIUtility.systemCopyBuffer));
                    if (obj != null)
                    {
                        var bt = obj.GetComponent<ButtonAnimation>();
                        if (bt != null)
                        {
                            EditorUtility.CopySerialized(bt, myButton);
                            myButton.gameObject.GetInstanceID().ToString().GBLog("LOAD BUTTON", Color.blue);

                        }
                    }
                }
                catch
                {
                    Debug.LogError("None Buffer");
                }
            }

            GB.EditorGUIUtil.End_Horizontal();

        }


        void DrawScale(ButtonAnimation button)
        {
        
            GB.EditorGUIUtil.Start_VerticalBox();

            GB.EditorGUIUtil.DrawSectionStyleLabel("Button UP");
            DrawScaleCustum(ButtonAnimation.BtnEventType.Up, button);


            GB.EditorGUIUtil.DrawSectionStyleLabel("Button DOWN");
            DrawScaleCustum(ButtonAnimation.BtnEventType.Down, button);

            GB.EditorGUIUtil.End_Vertical();
        }

        void DrawScaleCustum(ButtonAnimation.BtnEventType btnType, ButtonAnimation button)
        {
            GB.EditorGUIUtil.Start_Horizontal();
            bool isActive = button.ButtonData.BtnState[btnType];
            isActive = EditorGUILayout.Toggle("", isActive, GUILayout.Width(30));
            button.ButtonData.BtnState[btnType] = isActive;

            if (isActive)
            {
                var btnActStyle = button.ButtonData.BtnStyle[btnType];

                if (btnActStyle == ButtonAnimation.BtnActionStyle.Linear)
                    GB.EditorGUIUtil.BackgroundColor(Color.magenta);

                if (GB.EditorGUIUtil.DrawButton(ButtonAnimation.BtnActionStyle.Linear.ToString()))
                {
                    button.ButtonData.BtnStyle[btnType] = ButtonAnimation.BtnActionStyle.Linear;

                }
                GB.EditorGUIUtil.BackgroundColor(Color.white);

                if (btnActStyle == ButtonAnimation.BtnActionStyle.Punch)
                    GB.EditorGUIUtil.BackgroundColor(Color.magenta);

                if (GB.EditorGUIUtil.DrawButton(ButtonAnimation.BtnActionStyle.Punch.ToString()))
                {
                    button.ButtonData.BtnStyle[btnType] = ButtonAnimation.BtnActionStyle.Punch;

                }
                GB.EditorGUIUtil.BackgroundColor(Color.white);

                if (btnActStyle == ButtonAnimation.BtnActionStyle.Shake)
                    GB.EditorGUIUtil.BackgroundColor(Color.magenta);
                if (GB.EditorGUIUtil.DrawButton(ButtonAnimation.BtnActionStyle.Shake.ToString()))
                {
                    button.ButtonData.BtnStyle[btnType] = ButtonAnimation.BtnActionStyle.Shake;

                }
                GB.EditorGUIUtil.BackgroundColor(Color.white);
            }

            GB.EditorGUIUtil.End_Horizontal();

            if (isActive)
            {
                float duration = button.ButtonData.DurationValues[btnType];
                duration = EditorGUILayout.FloatField("Duration", duration);
                button.ButtonData.DurationValues[btnType] = duration;

                float power = button.ButtonData.PowerValues[btnType];
                power = EditorGUILayout.FloatField("Power", power);
                button.ButtonData.PowerValues[btnType] = power;
            }


        }





        public GameObject FindObject(int instanceID)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.GetInstanceID() == instanceID)
                {
                    return obj;
                }
            }
            return null;
        }
    }
}
#endif
