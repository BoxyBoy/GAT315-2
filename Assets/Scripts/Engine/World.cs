using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public FloatData gravity;
    public FloatData gravitation;
    public FloatData fixedFPS;
    public float timeAccumulator;
    public float fixedDeltaTime;
    public StringData fpsText;
    public BoolData collision;
    public BoolData wrap;
    public VectorField vectorField;
    
    static World instance;
    static public World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();
    public List<Spring> springs { get; set; } = new List<Spring>();
    public List<Force> forces { get; set; } = new List<Force>();

    public Vector2 WorldSize { get => size * 2; }
    public AABB AABB { get => aabb; }

    AABB aabb;
    Vector2 size;
    float fps;
    float fpsAverage;
    float smoothing;

    private void Awake()
    {
        instance = this;
        size = Camera.main.ViewportToWorldPoint(Vector2.one);
        aabb = new AABB(Vector2.zero, size * 2);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        fps = (1.0f / dt);

        //smoothing
        //fpsAverage = (fpsAverage * smoothing) + (fps * (1.0f - smoothing));
        //fpsText.value = "FPS: " + fpsAverage.ToString("F1");

        springs.ForEach(spring => spring.Draw());
        if (!simulate.value) return;

        GravitationalForce.ApplyForce(bodies, gravitation.value);
        forces.ForEach(force => bodies.ForEach(body => force.ApplyForce(body)));
        springs.ForEach(spring => spring.ApplyForce());
        bodies.ForEach(body => vectorField.ApplyForce(body));

        timeAccumulator += fixedDeltaTime;
        while (timeAccumulator >= fixedDeltaTime) 
        { 
            bodies.ForEach(body => body.Step(fixedDeltaTime)); 
            bodies.ForEach(body => Integrator.ExplicitEuler(body, fixedDeltaTime));

            bodies.ForEach(Body => Body.shape.color = Color.white);
            if(collision == true)
            {
                Collision.CreateContacts(bodies, out List<Contact> contacts);
                contacts.ForEach(contact => { contact.bodyA.shape.color = Color.red; contact.bodyB.shape.color = Color.red; });
                ContactSolver.Resolve(contacts);
            }

            timeAccumulator = timeAccumulator - fixedDeltaTime;
        }

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);
    }
}
