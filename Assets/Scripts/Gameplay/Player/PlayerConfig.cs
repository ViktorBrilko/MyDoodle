using System;
using Newtonsoft.Json;

[Serializable]
public class PlayerConfig
{
    public float JumpForce { get; set; }
    public float Speed { get; set; }
    public float JumpAidCoef { get; set; }
    public float GravityScale { get; set; }
    
    [JsonIgnore]
    public float MaxJumpHeight => JumpForce * JumpForce / (2 * 9.81f * GravityScale) * JumpAidCoef;
}