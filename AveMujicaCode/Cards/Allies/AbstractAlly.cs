using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public abstract class AbstractAlly : CustomMonsterModel
{
  public AbstractAlly()
  {
    CustomContentDictionary.RegisterType(GetType());
  }
  public override int MinInitialHp => 1;

  public override int MaxInitialHp => 1;

  public override bool IsHealthBarVisible => Creature.IsAlive;

  protected abstract MoveState GetDefaultMoveState();

  public override Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (NextMove != GetDefaultMoveState())
    {
      SetMoveImmediate(GetDefaultMoveState());
      SetUpSkill1Button();
      SetUpSkill2Button();
    }
    return Task.CompletedTask;
  }
  
  protected override MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    MoveState initialState = GetDefaultMoveState();
    initialState.FollowUpState = initialState;
    return new MonsterMoveStateMachine(
      new List<MonsterState> { initialState },
      initialState
    );
  }
  
  public override async Task BeforeSideTurnEndEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side == CombatSide.Player)
    {
      await PerformMove();
    }
  }

  protected void SetUpSkillButton(string path, int skillNum)
  {
    NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(Creature);
    Marker2D? specialNode = creatureNode?.GetSpecialNode<Marker2D>("%IntentPos");
    if (specialNode != null)
    {
      var buttonScene = GD.Load<PackedScene>("res://AveMujica/images/charui/ally_button.tscn");
      if (buttonScene != null)
      {
        var button = buttonScene.Instantiate<NAllyButton>();
        if (button != null)
        {
          TextureRect? textureNode = button.GetNodeOrNull<TextureRect>("%ButtonVisual");
          if (textureNode != null)
          {
            textureNode.Texture = GD.Load<Texture2D>(path);
          }
          button.owner = this;
          button.skillNum = skillNum;
          specialNode.AddChildSafely(button);
          if (skillNum == 1)
          {
            button.Position += new Vector2(-125f, 0f);
          }
          else
          {
            button.Position += new Vector2(-125f, 75f);
          }
        }
      }
    }
  }

  protected abstract void SetUpSkill1Button();
  
  protected abstract void SetUpSkill2Button();

  public abstract Task Skill1();
  
  public abstract Task Skill2();
}
