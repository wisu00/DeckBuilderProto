using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Card))]
public class CardCustomInspector : Editor {

    SerializedProperty cardTypeProp;

    void OnEnable() {
        cardTypeProp = serializedObject.FindProperty("cardType");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(cardTypeProp);

        string[] excludedFields = { "cardIsTargeted", "turnStartEffects", "turnEndEffects" };

        if ((CardType)cardTypeProp.enumValueIndex == CardType.CardBack) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cardArt"));
        }
        else if ((CardType)cardTypeProp.enumValueIndex == CardType.Identity) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("characterClass"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cardArt"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
        }
        else if ((CardType)cardTypeProp.enumValueIndex == CardType.Junk) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cardArt"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("discardValue"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("onHandChangesEffects"));
		}
        else {
            DrawPropertiesExcluding(serializedObject, excludedFields);

            if ((CardType)cardTypeProp.enumValueIndex == CardType.Tool || (CardType)cardTypeProp.enumValueIndex == CardType.Location) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("turnStartEffects"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("turnEndEffects"));
            }
            else if ((CardType)cardTypeProp.enumValueIndex == CardType.Event) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cardIsTargeted"));
            }
        }

        serializedObject.ApplyModifiedProperties();

    }
}
