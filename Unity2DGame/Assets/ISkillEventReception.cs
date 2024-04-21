
public interface ISkillEventReception
{
    public void InstallSkill(PlayerController player, WeaponController weapon, SkillData skillData);

    public void AnimationEventStart();
    public void AnimationEventInvoke();
    public void AnimationEventEnd();
}
