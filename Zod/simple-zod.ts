import { strict as assert } from 'assert';
import { z } from "zod";

/// Define a way to test types are equal

function assertTypes<T extends never>() {}
type TypeEqualityGuard<A,B> = Exclude<A,B> | Exclude<B,A>;

///   The pieces of Zod we are going to duplicate

const mySchema = z.object({
  firstname: z.string(),
  surname: z.string()
})

const test = mySchema.parse({
  firstname: "Clive",
  surname: "Tong"
});

assert.equal(test.firstname, "Clive");

assert.throws(() => {
  const test = mySchema.parse({
    firstname: 1,
    surname: 2
  });  
})

type SchemaT = z.infer<typeof mySchema>;

assertTypes<TypeEqualityGuard<SchemaT, { firstname: string, surname: string}>>();

const validInput : SchemaT = {
  firstname: "Clive",
  surname: "Tong"
};

/// Let's do strings first

class TypeHolder<Output> {
  readonly _output!: Output;
}

type Infer<T extends Parser<any>> = T["type"]

function isString(val: unknown): asserts val is string {
  if (typeof val != "string") throw `${val} is not a string`;
}

const x = {
  string: () => ({
    parse: (arg: unknown): string => {
      isString(arg);
      return arg;
    },
    type: new TypeHolder<string>()["_output"]
  })
}

const simpleSchema1 = x.string()

const test1 = simpleSchema1.parse("hello");

assert.equal(test1, "hello")

type Inferred1 = Infer<typeof simpleSchema1>

assertTypes<TypeEqualityGuard<Inferred1, string>>();

/// Do the same for numbers

function isNumber(val: unknown): asserts val is number {
  if (typeof val != "number") throw `${val} is not a number`;
}

const x1 = {
  number: () => ({
    parse: (arg: unknown): number => {
      isNumber(arg);
      return arg;
    },
    type: new TypeHolder<number>()["_output"]
  })
}

/// And now do objects

type Parser<T> = {
  parse: (arg: unknown) => T,
  type: unknown
}

type Schema<T> = Record<string, Parser<T>>;

const y = {

  ...x,

  ...x1,

  object: function <S extends Schema<unknown>>(schema: S) {

    function validateArgs(arg: unknown): asserts arg is {
      [K in keyof S]: ReturnType<S[K]["parse"]>
    } {

      if (!arg) throw "null"
      if (typeof arg != "object") throw "no object"

      for (const k of Object.keys(schema)) {
        if (k in arg) {
          schema[k].parse((arg as any)[k]);
        }
        else {
          throw "Invalid type"
        }
      }
    }

    return {

      parse(arg: unknown) {
        validateArgs(arg)
        return arg;
      },

      type: new TypeHolder<{ [K in keyof S]: S[K]["type"]}>()["_output"]
    }
  }
}

const simpleSchema2 = y.object({
  firstname: y.string(),
  surname: y.string()
})

const test2 = simpleSchema2.parse({
  firstname: "Clive",
  surname: "Tong"
})

assert.equal(test2.firstname, "Clive");

const simpleSchema3 = y.object({
  firstname: y.string(),
  surname: y.string(),
  address: y.object({
    number: y.number(),
    road: y.string(),
    town: y.string()
  })
})

const test3 = simpleSchema3.parse({
  firstname: "Clive",
  surname: "Tong",
  address: {
    number: 6,
    road: "big road",
    town: "Cambridge"
  }
})

assert.equal(test3.address.road, "big road");

type Inferred2 = Infer<typeof simpleSchema3>

assertTypes<TypeEqualityGuard<Inferred2, { firstname: string, surname: string, address: { number: number, road: string, town: string}}>>();

const test4 : Inferred2 = {
  firstname: "Clive",
  surname: "Tong",
  address: {
    number: 6,
    road: "big road",
    town: "Cambridge"
  }
};

