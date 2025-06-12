// Particle simulation compute shader
struct Particle
{
    float3 position;
    float3 velocity;
    float life;
    float age;
    float size;
    uint id;
};

RWStructuredBuffer<Particle> particles : register(u0);

cbuffer SimulationConstants : register(b0)
{
    float deltaTime;
    float3 gravity;
    float3 windForce;
    float dampening;
    uint particleCount;
    float3 emitterPos;
    float spawnRate;
}

// Simple hash function for randomness
float hash(uint n)
{
    n = (n << 13) ^ n;
    return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
}

float3 randomDirection(uint seed)
{
    float x = hash(seed * 73);
    float y = hash(seed * 137);
    float z = hash(seed * 241);
    return normalize(float3(x, y, z) * 2.0 - 1.0);
}

[shader("compute")]
[numthreads(64, 1, 1)]
void simulateParticles(uint3 id : SV_DispatchThreadID)
{
    if (id.x >= particleCount) return;
    
    Particle p = particles[id.x];
    
    if (p.age >= p.life)
    {
        // Respawn particle
        p.position = emitterPos + randomDirection(id.x + uint(deltaTime * 1000)) * 0.1;
        p.velocity = randomDirection(id.x * 311 + uint(deltaTime * 1000)) * 2.0;
        p.age = 0.0;
        p.life = 3.0 + hash(id.x * 97) * 2.0;
        p.size = 0.1 + hash(id.x * 131) * 0.05;
    }
    else
    {
        // Update existing particle
        p.age += deltaTime;
        
        // Apply forces
        float3 totalForce = gravity + windForce;
        p.velocity += totalForce * deltaTime;
        p.velocity *= pow(dampening, deltaTime);
        
        // Update position
        p.position += p.velocity * deltaTime;
    }
    
    particles[id.x] = p;
}

[shader("compute")]
[numthreads(64, 1, 1)]
void resetParticles(uint3 id : SV_DispatchThreadID)
{
    if (id.x >= particleCount) return;
    
    Particle p;
    p.position = emitterPos;
    p.velocity = float3(0, 0, 0);
    p.life = 3.0;
    p.age = 10.0; // Mark as dead so it respawns immediately
    p.size = 0.1;
    p.id = id.x;
    
    particles[id.x] = p;
}
