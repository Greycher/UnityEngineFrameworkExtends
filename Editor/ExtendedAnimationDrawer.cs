using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ExtendedAnimation.Animation))]
public class ExtendedAnimationDrawer : PropertyDrawer
{
    public int _statesCount;
    public bool _isStatesExpanded;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Rect curr = position;
        curr.height = EditorGUIUtility.singleLineHeight;
        
        Rect remainder = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        remainder.x -= curr.x - 4;
        property.isExpanded = EditorGUI.Foldout(new Rect(curr.x, curr.y, curr.width - remainder.width, curr.height), property.isExpanded, GUIContent.none);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            
            var animationPlayType = property.FindPropertyRelative("animationPlayType");
            
            curr.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(curr, animationPlayType);
            
            var animationStateType = property.FindPropertyRelative("animationStateType");
            
            curr.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(curr, animationStateType);
            
            curr.y += EditorGUIUtility.singleLineHeight;
            if (animationStateType.enumValueIndex == (int)ExtendedAnimation.AnimationStateType.One)
            {
                EditorGUI.PropertyField(curr, property.FindPropertyRelative("state"));
            }
            else
            {
                var states = property.FindPropertyRelative("states");
                if (EditorGUI.PropertyField(curr, property.FindPropertyRelative("states")))
                {
                    curr.y += EditorGUIUtility.singleLineHeight;
                    states.arraySize = EditorGUI.IntField(curr, "Size", states.arraySize);
                    for (int i = 0; i < states.arraySize; i++)
                    {
                        curr.y += EditorGUIUtility.singleLineHeight;
                        EditorGUI.PropertyField(curr, states.GetArrayElementAtIndex(i));
                    }
                }
                _statesCount = states.arraySize;
                _isStatesExpanded = states.isExpanded;
            }

            curr.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(curr, property.FindPropertyRelative("normalizedTransitionDuration"));
            
            curr.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(curr, property.FindPropertyRelative("layer"));
            
            curr.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(curr, property.FindPropertyRelative("minNormalizedTime"));
            
            curr.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(curr, property.FindPropertyRelative("maxNormalizedTime"));

            switch (@animationPlayType.enumValueIndex)
            {
                case (int)ExtendedAnimation.AnimationPlayType.WithSpeed:
                    curr.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(curr, property.FindPropertyRelative("speedMultiplierParam"));
                    
                    curr.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(curr, property.FindPropertyRelative("speed"));
                    break;
                
                case (int)ExtendedAnimation.AnimationPlayType.WithLength:
                    curr.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(curr, property.FindPropertyRelative("speedMultiplierParam"));
                    
                    curr.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(curr, property.FindPropertyRelative("length"));
                    break;
                
                case (int)ExtendedAnimation.AnimationPlayType.ScaleWithSpeed:
                    curr.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(curr, property.FindPropertyRelative("speedMultiplierParam"));
                    
                    curr.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(curr, property.FindPropertyRelative("speed"), new GUIContent("Base Speed"));
                    break;
            }
            
            EditorGUI.indentLevel--;
        }


        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lineCount;
        if (property.isExpanded)
        {
            lineCount = 10;
            var animationPlayType = property.FindPropertyRelative("animationPlayType");
            if (animationPlayType.enumValueIndex == (int) ExtendedAnimation.AnimationPlayType.DefaultSpeed) lineCount -= 2;
            
            var animationStateType = property.FindPropertyRelative("animationStateType");
            if (animationStateType.enumValueIndex == (int) ExtendedAnimation.AnimationStateType.RandomOneFromMultiple && _isStatesExpanded)
            {
                lineCount += _statesCount + 1;
            }
        }
        else
        {
            lineCount = 1;
        }
            
        return lineCount * EditorGUIUtility.singleLineHeight;
    }
}
