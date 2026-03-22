using System.Collections.Generic;
using UnityEngine;

public class NewspaperGenerator : MonoBehaviour
{
    [SerializeField] private NewspaperTemplateData templateData;

    public NewspaperIssue GenerateIssue(WarState warState, MissionData mission, MissionResult result, IReadOnlyList<string> decisions)
    {
        string headlineTemplate = SelectTemplate(templateData.headlineVariants);
        string bodyTemplate = SelectTemplate(templateData.bodyTemplates);

        Dictionary<string, string> variables = new Dictionary<string, string>
        {
            { "MissionTitle", mission != null ? mission.title : "Unassigned Operation" },
            { "PropagandaValue", result.propagandaScore.ToString() },
            { "WarScore", warState.warScore.ToString() },
            { "RegimeStability", warState.regimeStability.ToString() },
            { "CivilianUnrest", warState.civilianUnrest.ToString() },
            { "InternationalPressure", warState.internationalPressure.ToString() },
            { "FamilySafetyIndex", warState.familySafetyIndex.ToString() },
            { "LastChoice", decisions.Count > 0 ? decisions[decisions.Count - 1] : "None" }
        };

        return new NewspaperIssue
        {
            headline = InjectVariables(headlineTemplate, variables),
            body = InjectVariables(bodyTemplate, variables)
        };
    }

    private static string SelectTemplate(List<string> templates)
    {
        if (templates == null || templates.Count == 0)
        {
            return string.Empty;
        }

        return templates[Random.Range(0, templates.Count)];
    }

    private static string InjectVariables(string template, Dictionary<string, string> variables)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            return string.Empty;
        }

        string output = template;
        foreach (KeyValuePair<string, string> variable in variables)
        {
            output = output.Replace($"{{{variable.Key}}}", variable.Value);
        }

        return output;
    }
}

public struct NewspaperIssue
{
    public string headline;
    public string body;
}
