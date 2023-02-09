using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayerDto
{
    public int Id { get; set; }
    public string Account { get; set; }
    public string Name { get; set; }
    public bool RegisterImx { get; set; }
}

public class PlayerName
{
    public string Name { get; set; }
}