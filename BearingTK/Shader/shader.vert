#version 330 core

in vec3 aPosition;

in vec3 aNormal;

in vec2 aTexCoord;


out vec2 TexCoords;

out vec3 FragPos;

out vec3 Normal;


uniform mat4 model;

 uniform mat4 projection;

 uniform mat4 view;

void main(void)
{

    TexCoords = aTexCoord;

    FragPos = vec3(vec4(aPosition , 1.0) * model);

    gl_Position = vec4(aPosition, 1.0)  * model * view * projection ;

    Normal = aNormal * mat3(transpose(inverse(model)));


}