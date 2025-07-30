using FleetingCity.BAL.Enums;
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
                _animatedSprite.Play("idle");
                break;
            case MOVEMENT_DIRECTION_ENUM.UP:
                _animatedSprite.Play("walk_up");
                break;
            case MOVEMENT_DIRECTION_ENUM.DOWN:
                _animatedSprite.Play("walk_down");
                break;
            case MOVEMENT_DIRECTION_ENUM.LEFT:
                _animatedSprite.Play("walk_left");
                break;
            case MOVEMENT_DIRECTION_ENUM.RIGHT:
                _animatedSprite.Play("walk_right");
                break;
            case MOVEMENT_DIRECTION_ENUM.UP_RIGHT:
                _animatedSprite.Play("walk_up_right");
                break;
            case MOVEMENT_DIRECTION_ENUM.UP_LEFT:
                _animatedSprite.Play("walk_up_left");
                break;
            case MOVEMENT_DIRECTION_ENUM.DOWN_LEFT:
                _animatedSprite.Play("walk_down_left");
                break;
            case MOVEMENT_DIRECTION_ENUM.DOWN_RIGHT:
                _animatedSprite.Play("walk_down_right");
                break;
            default:
                _animatedSprite.Stop();
                break;
        }
    }

}
