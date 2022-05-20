using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbol : MonoBehaviour
{
    
    [SerializeField] private GameObject branchPrefab;

    [SerializeField] private int totalLevels = 2;

    [SerializeField] private float rootLenght = 4;

    [SerializeField] private float lengthReductionFactor = 0.2f;

    [SerializeField] private float minValidLenght = 0.1f;

    Queue<GameObject> frontier = new Queue<GameObject>();



    private int currentLevel = 0;
    private float currentLenght = -1;

    // Start is called before the first frame update
    void Start()
    {
        currentLenght = rootLenght;
        GameObject root = Instantiate(branchPrefab, transform);
        root.name = "Root branch";

        SetBranchLenght(root, currentLenght);

        frontier.Enqueue(root);
        ++currentLevel;

        GenerateTree();
         
    }

    private void GenerateTree() {
        if (currentLevel >= totalLevels) return;
        ++currentLevel;

        List<GameObject> createdBranches = new List<GameObject>();

        currentLenght -= rootLenght * lengthReductionFactor;

        currentLenght = Mathf.Max(currentLenght, minValidLenght);

        while (frontier.Count > 0)
        {
            var branch = frontier.Dequeue();

            var leftBranch = CreateBranch(branch, Random.Range(10f,30f));
            var rightBranch = CreateBranch(branch, -Random.Range(10f,30f));

            leftBranch.name = "Left branch";
            rightBranch.name = "Right branch";

            SetBranchLenght(leftBranch, currentLenght);
            SetBranchLenght(rightBranch, currentLenght);

            createdBranches.Add(leftBranch);
            createdBranches.Add(rightBranch);
        }

        foreach (var branch in createdBranches)
        {
            frontier.Enqueue(branch);
        }

        GenerateTree();
    }

    private GameObject CreateBranch(GameObject prevBranch, float angle = 0) {
        GameObject branch = Instantiate(prevBranch, transform);
        branch.transform.localPosition = prevBranch.transform.localPosition + prevBranch.transform.up * GetBranchLenght(prevBranch);
        branch.transform.rotation *= Quaternion.Euler(0, 0, angle);

        return branch;
    }

    private void SetBranchLenght(GameObject branch, float lenght) {
        Transform line = branch.transform.GetChild(0);
        Transform circle = branch.transform.GetChild(1);

        line.localScale = new Vector3(line.localScale.x, lenght, line.localScale.z);
        line.localPosition = new Vector3(line.localPosition.x, lenght/2f, line.localPosition.z);

        circle.localPosition = new Vector3(circle.localPosition.x, lenght, circle.localPosition.z);
    }

    private float GetBranchLenght(GameObject branch) {
        return branch.transform.GetChild(0).localScale.y;
    }
}
