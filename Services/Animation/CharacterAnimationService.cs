using FleetingCity.BAL.Enum;
using Godot;
using System;
using System.Linq;

public class CharacterAnimationService
{
    private Node2D _character;
    private AnimatedSprite2D _animatedSprite;
    private float _animationSpeed = 1.5f; // Adjust as needed
    public CharacterAnimationService(Node2D character)
    {
        _character = character;
        _animatedSprite = _character.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite.SpeedScale = _animationSpeed;
    }

    public void PlayWalkAnimation(MOVEMENT_DIRECTION_ENUM direction)
    {
        if (_animatedSprite == null)
            return;

        switch (direction)
        {
            case MOVEMENT_DIRECTION_ENUM.NONE:
                PlayAnimationWithFallback("idle", "walk");
                break;
            case MOVEMENT_DIRECTION_ENUM.UP:
                PlayAnimationWithFallback("walk_up");
                break;
            case MOVEMENT_DIRECTION_ENUM.DOWN:
                PlayAnimationWithFallback("walk_down");
                break;
            case MOVEMENT_DIRECTION_ENUM.LEFT:
                PlayAnimationWithFallback("walk_left");
                break;
            case MOVEMENT_DIRECTION_ENUM.RIGHT:
                PlayAnimationWithFallback("walk_right");
                break;
            case MOVEMENT_DIRECTION_ENUM.UP_RIGHT:
                PlayAnimationWithFallback("walk_up_right");
                break;
            case MOVEMENT_DIRECTION_ENUM.UP_LEFT:
                PlayAnimationWithFallback("walk_up_left");
                break;
            case MOVEMENT_DIRECTION_ENUM.DOWN_LEFT:
                PlayAnimationWithFallback("walk_down_left");
                break;
            case MOVEMENT_DIRECTION_ENUM.DOWN_RIGHT:
                PlayAnimationWithFallback("walk_down_right");
                break;
            default:
                _animatedSprite.Stop();
                break;
        }

    }

    private void PlayAnimationWithFallback(string animName, string fallback = "walk")
    {
        // Check if the animation exists in the sprite's frames
        if (_animatedSprite.SpriteFrames.HasAnimation(animName))
            _animatedSprite.Play(animName);
        else if (_animatedSprite.SpriteFrames.HasAnimation(fallback))
            _animatedSprite.Play(fallback);
        else
            GD.PrintErr($"Neither '{animName}' nor fallback '{fallback}' animation found!");
    }

}
