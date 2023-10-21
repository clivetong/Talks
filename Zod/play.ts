import { z } from "zod";

const mySchema = z.object({
  firstname: z.string(),
  surname: z.string()
})

const test = mySchema.parse({
  firstname: "Clive",
  surname: "Tong"
});


test.firstname;

; /////////////////////////////////////////////////////////////


function isString(val: unknown): asserts val is string {
  if (typeof val != "string") throw `${val} is not a string`;
}

const x = {
  string: () => ({
    parse: (arg: unknown): string => {
      isString(arg);
      return arg;
    }
  })
}

const simpleSchema1 = x.string()

const test1 = simpleSchema1.parse("hello");

; //////////////////////////////////////////////////////////

function isNumber(val: unknown): asserts val is number {
  if (typeof val != "number") throw `${val} is not a number`;
}

const add_numbers = {
  number: () => ({
    parse: (arg: unknown): number => {
      isNumber(arg);
      return arg;
    }
  })
}

  ; //////////////////////////////////////////////////////////


type Parser<T> = {
  parse: (arg: unknown) => T
}

type Schema<T> = Record<string, Parser<T>>;

const y = {

  ...x,

  ...add_numbers,

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
      }
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

test2.firstname;



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

test3.address.road;


