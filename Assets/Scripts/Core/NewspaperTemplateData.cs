using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewspaperTemplate", menuName = "WingsOfPeace/Newspaper Template")]
public class NewspaperTemplateData : ScriptableObject
{
    [TextArea(1, 3)] public List<string> headlineVariants = new List<string>();
    [TextArea(3, 8)] public List<string> bodyTemplates = new List<string>();
}
