grammar DfmGrammar;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}

object 
  : (OBJECT | INHERITED) (identifier COLON)? type (L_BRACKET index R_BRACKET)? ( property | object )* END
  ;

property 
  : qualifiedIdent EQ propertyValue
  ;

propertyValue 
  : identifier 
  | string 
  | number
  | set
  | itemList
  | stringList
  | binaryData
  | positionData
  ;

itemList 
  : LT item* GT
  ;

stringList
  : L_PAREN ( string )* R_PAREN
  ;

item 
  : ITEM ( property )* END
  ;

type 
  : identifier
  ;

set 
  : L_BRACKET identList R_BRACKET
  ;

positionData 
  : L_PAREN number* R_PAREN
  ;

identList 
  : identifier ( COMMA identifier )*
  ;

qualifiedIdent
  : identifier ( DOT identifier )*
  ;

index 
  : L_BRACKET number R_BRACKET
  ;

identifier
  : IDENT
  ;

string 
  : STRING_LITERAL (PLUS STRING_LITERAL)*
  ;

number 
  : MINUS? NUM_INT
  ;

binaryData
  : BINARY_DATA
  ;

OBJECT : 'object';
INHERITED : 'inherited';
ITEM : 'item';
END : 'end';

PLUS: '+';
MINUS: '-';
DOT: '.';
L_PAREN: '(';
R_PAREN: ')';
L_BRACKET: '[';
R_BRACKET: ']';
GT: '>';
LT: '<';
COMMA: ',';
COLON: ':';
EQ: '=';

BINARY_DATA
  : '{' ~('}')* '}'
  ;

IDENT
  : [a-zA-Z] [a-zA-Z0-9_]*
  ;

STRING_LITERAL
  : (QUOTED_STRING | ASCII_CODE)+
  ;
  
NUM_INT
  : [0-9]+ (('.' [0-9]+ (EXPONENT)?)? | EXPONENT)
  ;

fragment QUOTED_STRING
  : '\'' ( '\'\'' | ~('\'') )* '\''
  ;
   
fragment ASCII_CODE
  : '#' [0-9]+
  ;

fragment EXPONENT
  : 'e' [+-]? [0-9]+
  ;
  
WS : [ \t\r\n] -> skip ;