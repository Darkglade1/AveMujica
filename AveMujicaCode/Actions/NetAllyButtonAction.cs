using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace AveMujica.AveMujicaCode.Actions;

public struct NetAllyButtonAction : INetAction
{
  public int buttonNum;
  public ModelId modelId;

  public GameAction ToGameAction(Player player)
  {
    return new AllyButtonAction(player, buttonNum, modelId);
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(buttonNum);
    writer.WriteModelEntry(modelId);
  }

  public void Deserialize(PacketReader reader)
  {
    buttonNum = reader.ReadInt();
    modelId = reader.ReadModelIdAssumingType<MonsterModel>();
  }

  public override string ToString()
  {
    DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
    interpolatedStringHandler.AppendLiteral("NetAllyButtonAction:");
    interpolatedStringHandler.AppendLiteral("\nmodelId is " + modelId);
    interpolatedStringHandler.AppendLiteral("\nbuttonNum is " + buttonNum);
    return interpolatedStringHandler.ToStringAndClear();
  }
}
