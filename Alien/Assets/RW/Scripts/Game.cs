﻿/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int score = 0;
    public bool isGameOver = false;


    [SerializeField]
    private GameObject shipModel;
    [SerializeField]
    private GameObject startGameButton;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Spawner spawner;

    private static Game instance;

    [SerializeField]
    private Text lifesText;

    public int MaxLifes { get; } = 3;
    public int currentLifes = 3;

    [SerializeField]
    private int pointsToIncreaseDifficulty = 5;
    private int difficultyFlag = 0;


    [SerializeField]
    private int gameInitialDifficulty_AsteroidVel = 1;

    [SerializeField]
    private static float asteroidDifficultyIncrement = 0.2f;
    public static float Difficulty { get; set; }

    private void Start()
    {
        instance = this;
        titleText.enabled = true;
        gameOverText.enabled = false;
        scoreText.enabled = false;

        lifesText.enabled = false;

        startGameButton.SetActive(true);

        currentLifes = MaxLifes;


        Difficulty = gameInitialDifficulty_AsteroidVel;

        difficultyFlag = pointsToIncreaseDifficulty;
    }

    public static void GameOver()
    {
        instance.titleText.enabled = true;
        instance.gameOverText.enabled = true;

        instance.lifesText.enabled = false;

        instance.startGameButton.SetActive(true);
        instance.isGameOver = true;
        instance.spawner.StopSpawning();
        instance.shipModel.GetComponent<Ship>().Explode();

    }

    public void NewGame()
    {
        isGameOver = false;
        titleText.enabled = false;
        startGameButton.SetActive(false);
        shipModel.transform.position = new Vector3(0, -3.22f, 0);
        shipModel.transform.eulerAngles = new Vector3(90, 180, 0);
        score = 0;

        currentLifes = MaxLifes;

        scoreText.text = "Score: " + score;
        scoreText.enabled = true;

        spawner.BeginSpawning();
        shipModel.GetComponent<Ship>().RepairShip();
        spawner.ClearAsteroids();
        gameOverText.enabled = false;


        lifesText.text = "Lifes: " + shipModel.GetComponent<Ship>().CurrentLifes;
        lifesText.enabled = true;
    }

    public static void AsteroidDestroyed()
    {
        instance.score++;
        instance.scoreText.text = "Score: " + instance.score;

        if(instance.score >= instance.difficultyFlag)
        {
            IncreaseDifficulty();
            instance.difficultyFlag += instance.pointsToIncreaseDifficulty;
        }
    }

    public static void ShipDamaged()
    {
        instance.currentLifes--;
        instance.lifesText.text = "Lifes: " + instance.currentLifes; /*instance.shipModel.GetComponent<Ship>().CurrentLifes;*/
    }

    public Ship GetShip()
    {
        return shipModel.GetComponent<Ship>();
    }

    public Spawner GetSpawner()
    {
        return spawner.GetComponent<Spawner>();
    }

    private static void IncreaseDifficulty()
    {
        Difficulty += asteroidDifficultyIncrement;
    }

}
