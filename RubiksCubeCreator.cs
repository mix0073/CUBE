using UnityEngine;

[RequireComponent(typeof(RubiksCubeRotator))]
public class RubiksCubeCreator : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private GameObject smallCubePrefab;
    [SerializeField] private float cubeSize = 1f;
    [SerializeField][Range(0.01f, 0.1f)] private float gapSize = 0.05f;

    [Header("Face Materials")]
    [SerializeField] private Material[] faceMaterials; // Должно быть 6 материалов!

    private void Start()
    {
        ValidateMaterials();
        CreateVisibleCubes();
    }

    private void ValidateMaterials()
    {
        if (faceMaterials == null || faceMaterials.Length != 6)
        {
            Debug.LogError("Нужно 6 материалов для граней! Порядок: Front, Back, Left, Right, Up, Down");
            faceMaterials = new Material[6]; // Создаем пустой массив чтобы избежать ошибок
        }
    }

    private void CreateVisibleCubes()
    {
        // Центральные кубики (6 штук)
        for (int axis = 0; axis < 3; axis++)
        {
            CreateCube(GetAxisPosition(axis, 1), axis, 1);  // Положительное направление
            CreateCube(GetAxisPosition(axis, -1), axis, 0); // Отрицательное направление
        }

        // Рёберные кубики (12 штук) - без назначения материалов
        for (int axis1 = 0; axis1 < 3; axis1++)
        {
            for (int axis2 = 0; axis2 < 3; axis2++)
            {
                if (axis1 == axis2) continue;

                for (int sign1 = -1; sign1 <= 1; sign1 += 2)
                {
                    for (int sign2 = -1; sign2 <= 1; sign2 += 2)
                    {
                        CreateCube(GetEdgePosition(axis1, sign1, axis2, sign2), -1, -1);
                    }
                }
            }
        }

        // Угловые кубики (8 штук) - без назначения материалов
        for (int signX = -1; signX <= 1; signX += 2)
        {
            for (int signY = -1; signY <= 1; signY += 2)
            {
                for (int signZ = -1; signZ <= 1; signZ += 2)
                {
                    CreateCube(GetCornerPosition(signX, signY, signZ), -1, -1);
                }
            }
        }
    }

    private Vector3 GetAxisPosition(int axis, int sign)
    {
        Vector3 pos = Vector3.zero;
        pos[axis] = sign * (cubeSize - gapSize);
        return pos;
    }

    private Vector3 GetEdgePosition(int axis1, int sign1, int axis2, int sign2)
    {
        Vector3 pos = Vector3.zero;
        pos[axis1] = sign1 * (cubeSize - gapSize);
        pos[axis2] = sign2 * (cubeSize - gapSize);
        return pos;
    }

    private Vector3 GetCornerPosition(int signX, int signY, int signZ)
    {
        return new Vector3(
            signX * (cubeSize - gapSize),
            signY * (cubeSize - gapSize),
            signZ * (cubeSize - gapSize)
        );
    }

    private void CreateCube(Vector3 position, int axis, int materialIndex)
    {
        GameObject cube = Instantiate(smallCubePrefab, position, Quaternion.identity, transform);
        cube.name = $"Cube_{position.x}_{position.y}_{position.z}";

        // Назначаем материал только центральным кубикам
        if (axis >= 0 && materialIndex >= 0 && materialIndex < faceMaterials.Length)
        {
            MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
            if (faceMaterials[materialIndex] != null)
            {
                renderer.material = faceMaterials[materialIndex];
            }
        }
    }
}