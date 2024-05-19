#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Ews.Essentials.Data.FixedStrings;

internal abstract class FixedStringEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var buffer = property.FindPropertyRelative("buffer");
        var count = buffer.FindPropertyRelative("<Count>k__BackingField");

        List<SerializedProperty> props = new();
        int index = 0;
        while (true)
        {
            var prop = buffer.FindPropertyRelative($"_{index++}");
            if (prop != null)
            {
                props.Add(prop);
                continue;
            }
            break;
        }

        var strOld = new string(props.Take(count.intValue).Select(p => (char)p.intValue).ToArray());
        var strNew = EditorGUI.TextField(position, label, strOld);
        if (strNew != strOld)
        {
            count.intValue = strNew.Length;
            for (int i = 0; i < strNew.Length; i++)
            {
                if (i < props.Count)
                    props[i].intValue = strNew[i];
            }
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}

[CustomPropertyDrawer(typeof(FixedString8))]
internal class FixedStringEditor8 : FixedStringEditor { }

[CustomPropertyDrawer(typeof(FixedString32))]
internal class FixedStringEditor32 : FixedStringEditor { }

[CustomPropertyDrawer(typeof(FixedString128))]
internal class FixedStringEditor128 : FixedStringEditor { }
#endif