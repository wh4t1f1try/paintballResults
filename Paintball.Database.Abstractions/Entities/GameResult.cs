namespace Paintball.Database.Abstractions.Entities;

using System.ComponentModel.DataAnnotations;

public class GameResult
{
    public int Id { get; set; }


    public int GameDay { get; set; }


    [Required]
    public string TeamOne { get; set; } = null!;


    [Required]
    public string TeamTwo { get; set; } = null!;


    public int TeamOneMatchPoints { get; set; }


    public int TeamTwoMatchPoints { get; set; }
}