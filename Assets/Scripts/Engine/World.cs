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
    private Vector2 size;
    
    static World instance;
    static public World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();
    public List<Spring> springs { get; set; } = new List<Spring>();

    float fps;
    float fpsAverage;
    float smoothing;

    private void Awake()
    {
        instance = this;
        size = Camera.main.ViewportToWorldPoint(Vector2.one);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        fps = (1.0f / dt);

        //smoothing
        //fpsAverage = (fpsAverage * smoothing) + (fps * (1.0f - smoothing));
        //fpsText.value = "FPS: " + fpsAverage.ToString("F1");

        if (!simulate.value) return;

        GravitationalForce.ApplyForce(bodies, gravitation.value);
        springs.ForEach(spring => spring.ApplyForce());

        timeAccumulator += fixedDeltaTime;
        while (timeAccumulator > fixedDeltaTime) 
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
