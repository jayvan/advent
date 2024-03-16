use std::error::Error;
use std::io::stdin;

fn main() -> Result<(), Box<dyn Error>>{
    const WIDTH: usize = 25;
    const HEIGHT: usize = 6;
    const LAYER_SIZE: usize = WIDTH * HEIGHT;

    let mut buf = String::new();
    stdin().read_line(&mut buf)?;
    let line = buf.trim();

    let layers = line.len() / LAYER_SIZE;
    let mut output:[char;LAYER_SIZE] = [' ';LAYER_SIZE];

    for i in 0..layers {
        let layer = &line[i * LAYER_SIZE .. (i + 1) * LAYER_SIZE];
        for (i, j) in layer.chars().enumerate() {
            if output[i] == ' ' {
                output[i] = match j {
                   '0' => 'X',
                   '1' => '_',
                    _ => ' '
                };
            }
        }
    }

    for (i, c) in output.iter().enumerate() {
        print!("{c}");

        if (i + 1) % WIDTH == 0 {
            print!("\n");
        }
    }

    Ok(())
}
