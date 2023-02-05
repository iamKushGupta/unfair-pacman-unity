using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 4f;

    protected override void Eat()
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
        // Random rd = new Random();

        // int rand_num = rd.Next(0,3);
        // float gridSize = 20f;
        float randomX = UnityEngine.Random.Range(0,3);
        //int flag = random(0,1);
        if (randomX == 0) {
            FindObjectOfType<GameManager>().PacmanEaten();
        }
    }

}
