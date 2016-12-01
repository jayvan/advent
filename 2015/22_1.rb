#!/usr/bin/env ruby

$global_minimum = 2**15
boss_hp, $boss_dmg = ARGF.readlines.map { |line| line.split(' ').last.to_i }

def search(player_hp, mana, boss_hp, turn, shield, poison, recharge, mana_spent)
  # Handle start of turn effects
  if poison > 0
    poison -= 1
    boss_hp -= 3
  end

  if recharge > 0
    recharge -= 1
    mana += 101
  end

  if shield > 0
    shield -= 1
  end

  # Base case, boss dead
  if boss_hp <= 0
    if mana_spent < $global_minimum
      puts mana_spent
      $global_minimum = mana_spent
    end
    return mana_spent
  end

  return nil if mana_spent >= $global_minimum

  if turn == :boss
    player_hp -= $boss_dmg
    if shield > 0
      player_hp += 7
    end

    return nil if player_hp <= 0
    return search(player_hp, mana, boss_hp, :player, shield, poison, recharge, mana_spent)
  elsif turn == :player
    outcomes = []

    # Missile
    if mana >= 53
      outcomes << search(player_hp, mana - 53, boss_hp - 4, :boss, shield, poison, recharge, mana_spent + 53)
    end

    # Drain
    if mana >= 73
      outcomes << search(player_hp + 2, mana - 73, boss_hp - 2, :boss, shield, poison, recharge, mana_spent + 73)
    end

    # Shield
    if mana >= 113 && shield == 0
      outcomes << search(player_hp, mana - 113, boss_hp, :boss, 6, poison, recharge, mana_spent + 113)

    end

    # Poison
    if mana >= 173 && poison == 0
      outcomes << search(player_hp, mana - 173, boss_hp, :boss, shield, 6, recharge, mana_spent + 173)
    end

    # Recharge
    if mana >= 229 && recharge == 0
      outcomes << search(player_hp, mana - 229, boss_hp, :boss, shield, poison, 5, mana_spent + 229)
    end

    return outcomes.compact.min
  end
end

puts search(50, 500, boss_hp, :player, 0, 0, 0, 0)
