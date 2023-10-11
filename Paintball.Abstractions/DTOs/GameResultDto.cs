namespace Paintball.Abstractions.DTOs;

public class GameResultDto
{
    public int Id { get; set; }

    public int GameDay { get; set; }

    public string TeamOne { get; set; } = null!;

    public string TeamTwo { get; set; } = null!;

    public int TeamOneMatchPoints { get; set; }

    public int TeamTwoMatchPoints { get; set; }
}