pub fn get_digit(num: i32, digit: u32) -> u8 {
    ((num / (10_i32.pow(digit))) % 10) as u8
}

fn is_password(num: i32) -> bool {
    let mut has_double = false;
    let mut streak = 1;

    for i in 1..6 {
        if get_digit(num, i) == get_digit(num, i - 1) {
            streak += 1;
        }  else {
            // streak is over
            has_double = has_double || streak == 2;
            streak = 1;
        }

        if get_digit(num, i) > get_digit(num, i - 1) {
            return false;
        }
    }

    has_double || streak == 2
}


fn main() {
    let min = 240298;
    let max = 784956;
    let mut count = 0;
    for i in min..=max {
        if is_password(i) {
            println!("{i}");
            count += 1;
        }
    }
    println!("{count}");
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    pub fn examples() {
        assert!(is_password(112233));
        assert!(!is_password(123444));
        assert!(is_password(111122));
    }

    #[test]
    pub fn digit() {
        let num = 123456;
        assert_eq!(get_digit(num, 0), 6);
        assert_eq!(get_digit(num, 1), 5);
        assert_eq!(get_digit(num, 2), 4);
        assert_eq!(get_digit(num, 3), 3);
        assert_eq!(get_digit(num, 4), 2);
        assert_eq!(get_digit(num, 5), 1);
    }
}