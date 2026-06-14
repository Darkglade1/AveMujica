using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Audio;

namespace AveMujica.AveMujicaCode.Audio;

public static class Sfx
{
    public static readonly ModSound ATK_GUITAR = new("atk_guitar.ogg".AudioPath());
    public static readonly ModSound SKILL_GUITAR_VOCALS = new("skill_guitar_vocals.ogg".AudioPath());
    public static readonly ModSound SKILL_GUITAR_VOCALS2 = new("skill_guitar_vocals2.ogg".AudioPath());
    public static readonly ModSound SKILL_GUITAR_VOCALS3 = new("skill_guitar_vocals3.ogg".AudioPath());
    public static readonly ModSound SKILL_GUITAR = new("skill_guitar.ogg".AudioPath());
    public static readonly ModSound SKILL_BASS = new("skill_bass.ogg".AudioPath());
    public static readonly ModSound SKILL_DRUM = new("skill_drum.ogg".AudioPath());
    public static readonly ModSound SKILL_DRUM2 = new("skill_drum2.ogg".AudioPath());
    public static readonly ModSound SKILL_KEYBOARD2 = new("skill_keyboard2.ogg".AudioPath());
    public static readonly ModSound SKILL_KEYBOARD3 = new("skill_keyboard3.ogg".AudioPath());
}