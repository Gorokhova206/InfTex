-- Function: public.delete_file_by_id(integer)

-- DROP FUNCTION public.delete_file_by_id(integer);

CREATE OR REPLACE FUNCTION public.delete_file_by_id(integer)
  RETURNS void AS
$BODY$
BEGIN
	begin
		delete from file
		where id = $1;
		raise notice 'Удалено';
	exception when others then
		raise notice 'Ничего не удалено';
	end;
END;
 
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION public.delete_file_by_id(integer)
  OWNER TO postgres;
