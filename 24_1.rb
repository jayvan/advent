#!/usr/bin/env ruby

packages = []

ARGF.each_line do |line|
  packages << line.to_i
end

one_third = packages.inject(:+) / 3
done = false
lowest_entanglement = 2**64

1.upto(packages.length) do |size|
  packages.combination(size) do |combo|
    next unless combo.inject(:+) == one_third

    entanglement = combo.inject(:*)
    next unless entanglement < lowest_entanglement

    packages_left = packages - combo

    done_inner = false
    1.upto(packages_left.length) do |size_two|
      packages_left.combination(size_two).each do |combo_two|
        next unless combo_two.inject(:+) == one_third
        lowest_entanglement = [lowest_entanglement, combo.inject(:*)].min
        done = true
        done_inner = true
        break
      end
      break if done_inner
    end
  end

  break if done
end

puts lowest_entanglement
