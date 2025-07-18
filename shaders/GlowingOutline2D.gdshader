shader_type canvas_item;

uniform float outline_size = 2.0;
uniform vec4 outline_color = vec4(1.0, 0.0, 0.0, 1.0); // Red

void fragment() {
    vec4 tex = texture(TEXTURE, UV);
    bool draw_outline = false;

    if (tex.a < 0.1) {
        for (float x = -outline_size; x <= outline_size; x++) {
            for (float y = -outline_size; y <= outline_size; y++) {
                vec2 offset = vec2(x, y) / TEXTURE_PIXEL_SIZE;
                if (texture(TEXTURE, UV + offset).a > 0.1) {
                    draw_outline = true;
                }
            }
        }

        if (draw_outline) {
            COLOR = outline_color;
        } else {
            discard;
        }
    } else {
        COLOR = tex;
    }
}
