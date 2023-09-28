using System.ComponentModel.DataAnnotations;

namespace Paintball.Database.Abstractions.Entities;

public class GameResult
{
    public int Id { get; set; }

    public int Gameday { get; set; }

    [Required] public string TeamOne { get; set; }

    [Required] public string TeamTwo { get; set; }

    public int TeamOneMatchPoints { get; set; }

    public int TeamTwoMatchPoints { get; set; }
}